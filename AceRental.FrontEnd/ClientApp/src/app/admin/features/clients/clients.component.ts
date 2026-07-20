import {LiveAnnouncer} from '@angular/cdk/a11y';
import {AfterViewInit, Component, ElementRef, OnInit, ViewChild, inject} from '@angular/core';
import {MatSort, Sort, MatSortModule} from '@angular/material/sort';
import {MatTableDataSource, MatTableModule} from '@angular/material/table';
import { Router, RouterModule } from '@angular/router';
import { ClientService } from '../../../services/client.service';
import { ClientDto } from '../../../interfaces/client-dto';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { debounceTime, distinctUntilChanged, fromEvent, tap } from 'rxjs';

@Component({
  selector: 'app-clients.component',
  imports: [
    MatTableModule, 
    MatSortModule, 
    RouterModule, 
    MatProgressSpinnerModule, 
    MatInputModule, 
    MatFormFieldModule],
  templateUrl: './clients.component.html',
  styleUrl: './clients.component.css',
})
export class ClientsComponent implements OnInit, AfterViewInit {
  private _liveAnnouncer = inject(LiveAnnouncer);
  displayedColumns: string[] = ['ClientNumber', 'RaisonSociale', 'LastName', 'FirstName', 'Email'];
  dataSource = new MatTableDataSource<ClientDto>([]);
  @ViewChild('input') input: ElementRef | undefined;
  @ViewChild(MatSort) sort: MatSort | undefined;
  constructor(private clientService: ClientService, private router: Router) {
  }

  ngOnInit(): void {
    this.clientService.get().subscribe({
      next: clients => {
        this.dataSource.data = clients;
        console.log(clients);
      },
      error: err => console.error(err)
    });
  }
  
  ngAfterViewInit() {
      this.dataSource.sort = this.sort;
      fromEvent(this.input?.nativeElement,'keyup')
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
  onRowClicked(row : any) {
    const urlTree = this.router.createUrlTree(['/admin/client/', row.Id]);
    const url = this.router.serializeUrl(urlTree);
    window.open(window.location.origin + url, '_blank');
  }
}
