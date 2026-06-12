import {LiveAnnouncer} from '@angular/cdk/a11y';
import {AfterViewInit, Component, ElementRef, OnInit, ViewChild, inject} from '@angular/core';
import {MatSort, Sort, MatSortModule} from '@angular/material/sort';
import {MatTableDataSource, MatTableModule} from '@angular/material/table';
import { RouterModule, Router } from '@angular/router';
import { ReservationService } from '../../../services/reservation.service';
import { ReservationDetailsDto } from '../../../interfaces/reservation-details-dto';
import { DatePipe } from '@angular/common';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import { fromEvent } from 'rxjs/internal/observable/fromEvent';
import { debounceTime } from 'rxjs/internal/operators/debounceTime';
import { distinctUntilChanged } from 'rxjs/internal/operators/distinctUntilChanged';
import { tap } from 'rxjs/internal/operators/tap';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
@Component({
  selector: 'app-reservations.component',
  imports: [MatTableModule, MatSortModule, RouterModule, DatePipe, MatProgressSpinnerModule, MatInputModule, MatFormFieldModule],
  templateUrl: './reservations.component.html',
  styleUrl: './reservations.component.css',
})
export class ReservationsComponent implements OnInit, AfterViewInit  {
  reservationService: ReservationService
  private _liveAnnouncer = inject(LiveAnnouncer);
  displayedColumns: string[] = ['ReservationNumber', 'FirstName', 'LastName', 'RaisonSociale', 'StartDate', 'EndDate', 'TotalTTC', 'LogisticStatus', 'FinancialStatus'];
  dataSource = new MatTableDataSource<ReservationDetailsDto>([]);
  @ViewChild('input') input: ElementRef | undefined;
  @ViewChild(MatSort) sort: MatSort | undefined;

  constructor(private _reservationService: ReservationService, private _router: Router){
    this.reservationService = _reservationService;
    this.setupFilterPredicate();
  }

  setupFilterPredicate() {
    this.dataSource.filterPredicate = (data: ReservationDetailsDto, filter: string) => {
      const filterValue = filter.toLowerCase();
      return Object.values(data).some(value => {
        if (value === null || value === undefined) return false;
        if (typeof value === 'object') {
          return Object.values(value).some(v => String(v).toLowerCase().includes(filterValue));
        }
        return String(value).toLowerCase().includes(filterValue);
      });
    };
  }
  getData()
  {
    this.reservationService.get().subscribe({
      next: obj => {
        this.dataSource.data = obj;
        console.log(obj);
      },
      error: err => console.error(err)
    });
  }
  ngOnInit(): void {
    this.getData();
  }
  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    fromEvent(this.input?.nativeElement,'keyup')
            .pipe(
                debounceTime(150),
                distinctUntilChanged(),
                tap(() => {
                  const filterValue = this.input?.nativeElement.value || '';
                  console.log('Filter value:', filterValue);
                  this.dataSource.filter = filterValue;
                })
            )
            .subscribe();
  }

  announceSortChange(sortState: Sort) {
    if (sortState.direction) {
      this._liveAnnouncer.announce(`Sorted ${sortState.direction}ending`);
    } else {
      this._liveAnnouncer.announce('Sorting cleared');
    }
  }
  onRowClicked(row : any) {
    const urlTree = this._router.createUrlTree(['/admin/reservation/', row.Id]);
    const url = this._router.serializeUrl(urlTree);
    window.open(window.location.origin + url, '_blank');
  }
}
