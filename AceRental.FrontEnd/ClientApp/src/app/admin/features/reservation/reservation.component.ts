import { ChangeDetectorRef, Component, ElementRef, inject, OnInit, signal, ViewChild } from '@angular/core';
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { provideNativeDateAdapter, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { forkJoin } from 'rxjs';
import { ReservationService } from '../../../services/reservation.service';
import { CommonModule, formatDate } from '@angular/common';
import { ReservationDetailsDto } from '../../../interfaces/reservation-details-dto';
import { ReservationServicesDto } from '../../../interfaces/reservation-services-dto';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { LiveAnnouncer } from '@angular/cdk/a11y';
import { MatSort, Sort, MatSortModule } from '@angular/material/sort';
import { Workflow } from '../../../Enum/workflow';
import { FinancialStatus } from '../../../Enum/financial-status';
import { LogisticStatus } from '../../../Enum/logistic-status';
import { PackService } from '../../../services/pack.service';
import { EquipmentService } from '../../../services/equipment.service';
import { ServiceService } from '../../../services/service.service';
import { ReservationItemDto } from '../../../interfaces/reservation-item-dto';
import { ReservationPacksDto } from '../../../interfaces/reservation-packs-dto';
import { ReservationEquipmentsDto } from '../../../interfaces/reservation-equipments-dto';
import { BootstrapToastService } from '../../../services/bootstrap-toast.service';
import { ClientService } from '../../../services/client.service';
import { ClientDto } from '../../../interfaces/client-dto';
import {MatButtonToggleModule} from '@angular/material/button-toggle';
import { ServiceType } from '../../../Enum/service-type';

@Component({
  selector: 'app-reservation.component',
  providers: [{ provide: MAT_DATE_LOCALE, useValue: 'fr-FR' }, provideNativeDateAdapter()],
  imports: [
    MatFormFieldModule,
    MatDatepickerModule,
    FormsModule,
    MatInputModule,
    MatAutocompleteModule,
    ReactiveFormsModule,
    RouterModule,
    CommonModule,
    MatTableModule,
    MatSortModule,
    MatButtonToggleModule
  ],
  templateUrl: './reservation.component.html',
  styleUrl: './reservation.component.css',
})
export class ReservationComponent implements OnInit {
  public readonly FinancialStatus = FinancialStatus;
  public readonly LogisticStatus = LogisticStatus;
  public readonly Workflow = Workflow;

  id!: string;
  isCreatingMode: boolean = false;
  hideSingleSelectionIndicator = signal(true);
  reservation = signal<any>(null);
  reservationService: ReservationService;
  packService: PackService;
  equipmentService: EquipmentService;
  serviceService: ServiceService;
  clientService: ClientService;
  transportInclus: boolean = false;
  private _liveAnnouncer = inject(LiveAnnouncer);
  private toastService = inject(BootstrapToastService);
  displayedColumns: string[] = ['Reference', 'Name', 'Quantity', 'UnitPrice', 'Type', 'TotalHT', 'Delete'];
  dataSource = new MatTableDataSource<any>([]);
  @ViewChild('input') input: ElementRef | undefined;
  @ViewChild(MatSort) sort: MatSort | undefined;

  searchProduct = new FormControl('');
  lstProduct: ReservationItemDto[] = [];
  top5Product: ReservationItemDto[] = [];
  selectedItem: ReservationItemDto | null = null;


  searchClient = new FormControl('');
  lstClients: ClientDto[] = [];
  top5Clients: ClientDto[] = [];
  selectedClient: ClientDto | undefined;
  
  newQuantity = 1;
  newUnitPrice = 0;
  addedItems: any[] = [];


  // num = new FormControl(1);
  readonly range = new FormGroup({
    start: new FormControl<Date | null>(null),
    end: new FormControl<Date | null>(null),
    startTime: new FormControl<string | null>(null),
    endTime: new FormControl<string | null>(null),
  });

  constructor(
    private cdRef: ChangeDetectorRef,
    route: ActivatedRoute,
    private _reservationService: ReservationService,
    private _packService: PackService,
    private _equipmentService: EquipmentService,
    private _serviceService: ServiceService,
    private _clientService: ClientService
  ) {
    this.id = route.snapshot.paramMap.get('id') || '';
    this.reservationService = _reservationService;
    this.packService = _packService;
    this.equipmentService = _equipmentService;
    this.serviceService = _serviceService;
    this.clientService = _clientService;

    // this.setupFilterPredicate();
  }

  ngOnInit(): void {
    this.initlstProduct();
    this.initLstClient();
    this.loadReservation();


    console.log('this.reservation()', this.reservation());
    console.log('isCreatingMode:', this.isCreatingMode);
  }

  plus(item: any) {
    item.Quantity = Number(item.Quantity) + 1;
    this.calculeMontantTotale();
  }
  moins(item: any) {
    item.Quantity = Math.max(1, Number(item.Quantity) - 1);
    this.calculeMontantTotale();
  }
  onLivraisonInclusChecked(event: Event) {
    this.transportInclus = (event.target as HTMLInputElement).checked;

    console.log(' livraisonInclusChecked =', this.transportInclus);
  }

  calculeMontantTotale() {
    // const totalPacks = this.reservation.Packs.reduce((total, packReservation) => {
    //   return total + packReservation.pack.prix * packReservation.quantite;
    // }, 0);
    // const totalProduits = this.reservation.Equipments.reduce((total, produitReservation) => {
    //   return total + produitReservation.produit.DailyPriceHT * produitReservation.quantite;
    // }, 0);
    // this.reservation.totale = totalPacks + totalProduits;
  }

  initLstClient() {
    this._clientService.get().subscribe({
      next: clients => {
        this.lstClients = clients;
        this.top5Clients = this.top5Clients.slice(0, 5);
      },
      error: err => {
        console.error('Erreur loading items', err);
        const errorMessage = err?.error?.detail || err?.message || 'Erreur lors du chargement des clients';
        const errorTitle = err?.error?.title || err?.statusText || 'Erreur';
        this.toastService.error(errorTitle, errorMessage, err.status, 5000);
      }
    });

    this.searchClient.valueChanges.subscribe(value => {
      this.top5Clients = this.getFilteredClients(value);
    });
  }

  initlstProduct() {
    forkJoin({
      packs: this._packService.get(),
      equipments: this._equipmentService.get(),
      services: this._serviceService.get()
    }).subscribe({
      next: ({ packs, equipments, services }) => {
        this.lstProduct = [
          ...packs.map(pack => ({
            Id: pack.Id,
            Reference: pack.Reference,
            Name: pack.Name,
            UnitPrice: pack.DailyPriceHT,
            IsDailyPrice: true,
            Type: 'Pack' as const
          })),
          ...equipments.map(equipment => ({
            Id: equipment.Id.toString(),
            Reference: equipment.Reference,
            Name: equipment.Name,
            UnitPrice: equipment.DailyPriceHT,
            IsDailyPrice: true,
            Type: 'Equipment' as const
          })),
          ...services.map(service => ({
            Id: service.Id,
            Reference: service.Reference,
            Name: service.Name,
            UnitPrice: service.PriceHT,
            IsDailyPrice: service.IsDailyPrice,
            Type: 'Service' as const
          }))
        ];

        this.top5Product = this.lstProduct.slice(0, 5);
      },
      error: err => {
        console.error('Erreur loading items', err);
        const errorMessage = err?.error?.detail || err?.message || 'Erreur lors du chargement des articles';
        const errorTitle = err?.error?.title || err?.statusText || 'Erreur';
        this.toastService.error(errorTitle, errorMessage, err.status, 5000);
      }
    });

    this.searchProduct.valueChanges.subscribe(value => {
      this.top5Product = this.getFilteredProducts(value);
    });
  }

  initReservation() {
    let client: ClientDto = {
        ClientNumber: '',
        RaisonSociale: '',
        FirstName: '',
        LastName: '',
        Email: '',
        Password: '',
        Civilite: 'H',
        TelNumber: '',
        PhoneNumber: '',
        Address: '',
        ComplementAdresse: '',
        PostalCode: 0,
        City: '',
        CreateAt: new Date()
      };
      let res: ReservationDetailsDto = {
        Id: '',
        StartDate: new Date(),
        EndDate: new Date(),
        FinancialStatus: FinancialStatus.Unpaid,
        LogisticStatus: LogisticStatus.Draft,
        Workflow: Workflow.B2B,
        TotalHT: 0,
        TVA: 0,
        TotalTTC: 0,
        ClientId: this.selectedClient?.Id || client?.Id || "",
        Client: this.selectedClient || client,
        CurrentVersion: 1,
        Equipments: [],
        Packs: [],
        Services: []
      }
      this.reservation.set(res);
      // this.initlstProduct();
      this.dataSource.data = this.createDetailItems(res);
  }
  loadReservation() {
    if (!this.id) {
      this.isCreatingMode = true;
      this.initReservation();
    }
    else {
      this.reservationService.getById(this.id).subscribe({
        next: obj => {
          if (!obj) {
            this.toastService.error('Erreur', 'Reservation introuvable.', 204, 5000);
            throw new Error('Reservation introuvable.');
          }

          console.log('Response from API:', obj);
          this.reservation.set(obj);
          this.range.setValue({
            start: new Date(obj.StartDate),
            end: new Date(obj.EndDate),
            startTime: new Date(obj.StartDate)?.getHours()?.toString().padStart(2, '0') + ':' + new Date(obj.StartDate)?.getMinutes()?.toString().padStart(2, '0'),
            endTime: new Date(obj.EndDate)?.getHours()?.toString().padStart(2, '0') + ':' + new Date(obj.EndDate)?.getMinutes()?.toString().padStart(2, '0')
          });
          this.transportInclus = obj?.Services.some((s: ReservationServicesDto) => s.Service!.Type.toString() === ServiceType[ServiceType.Transport]) || false;
          // this.initlstProduct();
          this.dataSource.data = this.createDetailItems(obj);
        },
        error: err => {
          console.error(err);
          const errorMessage = err?.error?.detail || err?.message || 'Erreur lors du chargement de la réservation';
          const errorTitle = err?.error?.title || err?.statusText || 'Erreur';
          this.toastService.error(errorTitle, errorMessage, err.status, 5000);
        }
      });
    }
  }
  createDetailItems(reservation: ReservationDetailsDto) {
    const items: any[] = [];

    reservation.Packs?.forEach(pack => {
      items.push({
        Id: pack.Pack?.Id || pack.PackId,
        Reference: pack.Pack?.Reference,
        Name: pack.Pack?.Name,
        Quantity: pack.Quantity,
        UnitPrice: pack.UnitPriceAtTimeOfBooking,
        Type: 'Pack',
        TotalHT: pack.UnitPriceAtTimeOfBooking * pack.Quantity
      });
    });

    reservation.Equipments?.forEach(equipment => {
      items.push({
        Id: equipment.EquipmentId,
        Reference: equipment.Equipment?.Reference || equipment.EquipmentId,
        Name: equipment.Equipment?.Name || `Equipment ${equipment.EquipmentId}`,
        Quantity: equipment.Quantity,
        UnitPrice: equipment.UnitPriceAtTimeOfBooking,
        Type: 'Equipment',
        TotalHT: equipment.UnitPriceAtTimeOfBooking * equipment.Quantity
      });
    });

    reservation.Services?.forEach(service => {
      items.push({
        Id: service.ServiceId,
        Reference: service.Service?.Reference || service.ServiceId,
        Name: service.Service?.Name || 'Service',
        Quantity: service.Quantity,
        UnitPrice: service.UnitPriceAtTimeOfBooking,
        Type: 'Service',
        TotalHT: service.UnitPriceAtTimeOfBooking * service.Quantity
      });
    });

    return [...items, ...this.addedItems];
  }

  getFilteredProducts(query: string | null) {
    const value = String(query || '').toLowerCase();
    return this.lstProduct
      .filter(item =>
        item.Reference?.toLowerCase().includes(value) ||
        item.Name?.toLowerCase().includes(value) ||
        item.Type?.toLowerCase().includes(value)
      )
      .slice(0, 5);
  }
  getFilteredClients(query: string | null) {
    const value = String(query || '').toLowerCase();
    return this.lstClients
      .filter(client =>
        client.ClientNumber?.toString().toLowerCase().includes(value) ||
        client.RaisonSociale?.toLowerCase().includes(value) ||
        client.FirstName?.toLowerCase().includes(value) ||
        client.LastName?.toLowerCase().includes(value)
      )
      .slice(0, 5);
  }

  selectProduct(option: any) {
    this.selectedItem = option;
    this.newQuantity = 1;
    this.newUnitPrice = option.UnitPrice || 0;
  }
  selectClient(option: any) {
    this.selectedClient = option;
    this.initReservation();
    console.log('Selected client:', this.selectedClient);
  }

  addSelectedItem() {
    if (!this.selectedItem) {
      return;
    }

    const item = {
      Id: this.selectedItem.Id,
      Reference: this.selectedItem.Reference,
      Name: this.selectedItem.Name,
      Quantity: this.newQuantity,
      UnitPrice: this.newUnitPrice,
      Type: this.selectedItem.Type,
      TotalHT: this.newQuantity * this.newUnitPrice
    };

    this.addedItems.push(item);
    this.dataSource.data = [...this.dataSource.data, item];
    this.selectedItem = null;
    this.searchProduct.setValue('');
    this.newQuantity = 1;
    this.newUnitPrice = 0;
  }
  deleteSelectedItem(element: any) {
    if (!element) {
      return;
    }
    console.log('Deleting item:', element);
    console.log('this.addedItems before deletion:', this.addedItems);
    this.dataSource.data = this.dataSource.data.filter(item => item.Id !== element.Id);

    // console.log('addedItems after deletion:', this.addedItems);
    // this.dataSource.data = [...this.addedItems];
  }
  updateRow(row: any) {
    row.TotalHT = row.Quantity * row.UnitPrice;
    this.dataSource.data = [...this.dataSource.data];
  }
  announceSortChange(sortState: Sort) {
    if (sortState.direction) {
      this._liveAnnouncer.announce(`Sorted ${sortState.direction}ending`);
    } else {
      this._liveAnnouncer.announce('Sorting cleared');
    }
  }
  onRowClicked(row: any) {

  }
  onValidateChanges() {
    // Filtrer les packs de this.dataSource.data
    const packs: ReservationPacksDto[] = this.dataSource.data
      .filter(item => item.Type === 'Pack')
      .map(item => ({
        PackId: item.Id,
        Quantity: item.Quantity,
        UnitPriceAtTimeOfBooking: item.UnitPrice
      }));
    const equipments: ReservationEquipmentsDto[] = this.dataSource.data
      .filter(item => item.Type === 'Equipment')
      .map(item => ({
        EquipmentId: item.Id,
        Quantity: item.Quantity,
        UnitPriceAtTimeOfBooking: item.UnitPrice
      }));
    const services: ReservationServicesDto[] = this.dataSource.data
      .filter(item => item.Type === 'Service')
      .map(item => ({
        ServiceId: item.Id,
        Quantity: item.Quantity,
        UnitPriceAtTimeOfBooking: item.UnitPrice
      }));


    let starDateTime: Date = new Date(
      this.range.value.start?.getFullYear() || 0,
      this.range.value.start?.getMonth() || 0,
      this.range.value.start?.getDate() || 0,
      this.range.value.startTime?.indexOf(":")! > 0 ? parseInt(this.range.value.startTime?.split(":")[0]!) : 0,
      this.range.value.startTime?.indexOf(":")! > 0 ? parseInt(this.range.value.startTime?.split(":")[1]!) : 0,
      0
    );
    let endDateTime: Date = new Date(
      this.range.value.end?.getFullYear() || 0,
      this.range.value.end?.getMonth() || 0,
      this.range.value.end?.getDate() || 0,
      this.range.value.endTime?.indexOf(":")! > 0 ? parseInt(this.range.value.endTime?.split(":")[0]!) : 0,
      this.range.value.endTime?.indexOf(":")! > 0 ? parseInt(this.range.value.endTime?.split(":")[1]!) : 0,
      0
    );

    this.range.setValue({
      start: starDateTime,
      end: endDateTime,
      startTime: this.range.value?.startTime || "00:00",
      endTime: this.range.value?.endTime || "00:00"
    });

    // console.log('this.range.value:', this.range.value);
    const res: ReservationDetailsDto = {
      ClientId: this.reservation().ClientId,
      // StartDate: this.range.value.start || new Date(),
      // EndDate: this.range.value.end || new Date(),
      StartDate: starDateTime,
      EndDate: endDateTime,
      Workflow: this.reservation().Workflow,
      Equipments: equipments,
      Packs: packs,
      Services: services
    };
    if (this.isCreatingMode) {
      this.createReservation(res);
    }
    else {
      this.updateReservation(res);
    }

    console.log('Validating changes...', res);
  }
  createReservation(res: ReservationDetailsDto) {
    this.reservationService.post(res).subscribe({
      next: createdRes => {
        console.log('Reservation created successfully', createdRes);
        this.toastService.success('Succès', 'Réservation créée avec succès', 5000);
        // this.reservation.set(createdRes);
        // this.dataSource.data = this.createDetailItems(createdRes);
      },
      error: err => {
        console.error('Error creating reservation', err);
        const errorMessage = err?.error?.detail || err?.message || 'Erreur lors de la création de la réservation';
        const errorTitle = err?.error?.title || err?.statusText || 'Erreur';
        this.toastService.error(errorTitle, errorMessage, err.status, 5000);
      }
    });
  }
  updateReservation(res: ReservationDetailsDto) {
    this.reservationService.patch(this.reservation().Id, res).subscribe({
      next: updatedRes => {
        console.log('Reservation updated successfully', updatedRes);
        this.toastService.success('Succès', 'Réservation mise à jour avec succès', 5000);
        // this.reservation.set(updatedRes);
        // this.dataSource.data = this.createDetailItems(updatedRes);
      },
      error: err => {
        console.error('Error updating reservation', err);
        const errorMessage = err?.error?.detail || err?.message || 'Erreur lors de la mise à jour de la réservation';
        const errorTitle = err?.error?.title || err?.statusText || 'Erreur';
        this.toastService.error(errorTitle, errorMessage, err.status, 5000);
      }
    });
  }
}


