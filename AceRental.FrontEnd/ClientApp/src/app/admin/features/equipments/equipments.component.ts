import { LiveAnnouncer } from '@angular/cdk/a11y';
import { AfterViewInit, Component, ElementRef, OnInit, ViewChild, inject } from '@angular/core';
import { MatSort, Sort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { Router, RouterModule } from '@angular/router';
import { EquipmentService } from '../../../services/equipment.service';
import { EquipmentsDto } from '../../../interfaces/equipment-dto';
import { debounceTime, distinctUntilChanged, fromEvent, tap } from 'rxjs';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-equipments',
  imports: [
    MatTableModule, 
    MatSortModule, 
    RouterModule, 
    MatProgressSpinnerModule, 
    MatInputModule, 
    MatFormFieldModule],
  templateUrl: './equipments.component.html',
  styleUrl: './equipments.component.css',
})
export class EquipmentsComponent implements OnInit, AfterViewInit {
  private _liveAnnouncer = inject(LiveAnnouncer);
  displayedColumns: string[] = ['Reference', 'Name', 'DailyPriceHT', 'TotalStock', 'Category'];
  dataSource = new MatTableDataSource<EquipmentsDto>([]);
  @ViewChild('input') input: ElementRef | undefined;
  @ViewChild(MatSort) sort: MatSort | undefined;
  constructor(private equipmentService: EquipmentService, private router: Router) {
  }
  ngOnInit(): void {
    this.equipmentService.get().subscribe({
      next: equipments => {
        this.dataSource.data = equipments;
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
  onRowClicked(row : any) {
    const urlTree = this.router.createUrlTree(['/admin/equipment/', row.Id]);
    const url = this.router.serializeUrl(urlTree);
    window.open(window.location.origin + url, '_blank');
  }
}
