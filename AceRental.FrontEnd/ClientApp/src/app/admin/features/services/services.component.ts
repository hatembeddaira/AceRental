import { LiveAnnouncer } from '@angular/cdk/a11y';
import { AfterViewInit, Component, ElementRef, OnInit, ViewChild, inject } from '@angular/core';
import { MatSort, Sort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { Router, RouterModule } from '@angular/router';
import { debounceTime, distinctUntilChanged, fromEvent, tap } from 'rxjs';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ServiceDto } from '../../../interfaces/service-dto';
import { ServiceService } from '../../../services/service.service';

@Component({
  selector: 'app-services',
  imports: [
    MatTableModule, 
    MatSortModule, 
    RouterModule, 
    MatProgressSpinnerModule, 
    MatInputModule, 
    MatFormFieldModule],
  templateUrl: './services.component.html',
  styleUrl: './services.component.css',
})
export class ServicesComponent implements OnInit, AfterViewInit {
private _liveAnnouncer = inject(LiveAnnouncer);
  displayedColumns: string[] = ['Reference', 'Name', 'PriceHT', 'IsDailyPrice', 'Type'];
  dataSource = new MatTableDataSource<ServiceDto>([]);
  @ViewChild('input') input: ElementRef | undefined;
  @ViewChild(MatSort) sort: MatSort | undefined;
  constructor(private serviceService: ServiceService, private router: Router) {
  }
  ngOnInit(): void {
    this.serviceService.get().subscribe({
      next: obj => {
        this.dataSource.data = obj;
      },
      error: err => console.error(err)
    });
  }
  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    fromEvent(this.input?.nativeElement, 'keyup')
      .pipe(
        debounceTime(150),
        distinctUntilChanged(),
        tap(() => {
          const filterValue = this.input?.nativeElement.value || '';
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
    const urlTree = this.router.createUrlTree(['/admin/service/', row.Id]);
    const url = this.router.serializeUrl(urlTree);
    window.open(window.location.origin + url, '_blank');
  }
}
