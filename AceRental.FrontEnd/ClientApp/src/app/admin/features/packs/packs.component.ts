import { LiveAnnouncer } from '@angular/cdk/a11y';
import { AfterViewInit, Component, ElementRef, OnInit, ViewChild, inject } from '@angular/core';
import { MatSort, Sort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { Router, RouterModule } from '@angular/router';
import { debounceTime, distinctUntilChanged, fromEvent, tap } from 'rxjs';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { PackService } from '../../../services/pack.service';
import { PackDetailsDto } from '../../../interfaces/pack-details-dto';

@Component({
  selector: 'app-packs',
  imports: [
    MatTableModule, 
    MatSortModule, 
    RouterModule, 
    MatProgressSpinnerModule, 
    MatInputModule, 
    MatFormFieldModule],
  templateUrl: './packs.component.html',
  styleUrl: './packs.component.css',
})
export class PacksComponent implements OnInit, AfterViewInit {
  private _liveAnnouncer = inject(LiveAnnouncer);
  displayedColumns: string[] = ['Reference', 'Name', 'DailyPriceHT', 'TotalStock', 'Category'];
  dataSource = new MatTableDataSource<PackDetailsDto>([]);
  @ViewChild('input') input: ElementRef | undefined;
  @ViewChild(MatSort) sort: MatSort | undefined;
  constructor(private packService: PackService, private router: Router) {
  }
  ngOnInit(): void {
    this.packService.get().subscribe({
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
    const urlTree = this.router.createUrlTree(['/admin/pack/', row.Id]);
    const url = this.router.serializeUrl(urlTree);
    window.open(window.location.origin + url, '_blank');
  }
}
