import {LiveAnnouncer} from '@angular/cdk/a11y';
import {AfterViewInit, Component, ViewChild, inject} from '@angular/core';
import {MatSort, Sort, MatSortModule} from '@angular/material/sort';
import {MatTableDataSource, MatTableModule} from '@angular/material/table';
import { RouterModule } from '@angular/router';
import { ReservationService } from '../../../../services/reservation.service';
import { ReservationDetailsDto } from '../../../../interfaces/reservation-details-dto';

@Component({
  selector: 'app-reservations.component',
  imports: [MatTableModule, MatSortModule, RouterModule],
  templateUrl: './reservations.component.html',
  styleUrl: './reservations.component.css',
})
export class ReservationsComponent {
  reservationService: ReservationService
  private _liveAnnouncer = inject(LiveAnnouncer);
  displayedColumns: string[] = ['ReservationNumber', 'FirstName', 'LastName', 'raisonSociale', 'dateCreation', 'StartDate', 'EndDate', 'derniereModification', 'TotalTTC', 'LogisticStatus', 'FinancialStatus', 'edit'];
  dataSource = new MatTableDataSource<ReservationDetailsDto>([]);

  constructor(private _reservationService: ReservationService){
    this.reservationService = _reservationService;
  }
ngOnInit(): void {
    this.reservationService.get().subscribe({
      next: obj => {
        this.dataSource.data = obj;
        console.log(obj);
      },
      error: err => console.error(err)
    });
  }
  @ViewChild(MatSort) sort: MatSort | undefined;
  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }

   /** Announce the change in sort state for assistive technology. */
  announceSortChange(sortState: Sort) {
    // This example uses English messages. If your application supports
    // multiple language, you would internationalize these strings.
    // Furthermore, you can customize the message to add additional
    // details about the values being sorted.
    if (sortState.direction) {
      this._liveAnnouncer.announce(`Sorted ${sortState.direction}ending`);
    } else {
      this._liveAnnouncer.announce('Sorting cleared');
    }
  }
}
