import {LiveAnnouncer} from '@angular/cdk/a11y';
import {AfterViewInit, Component, ViewChild, inject} from '@angular/core';
import {MatSort, Sort, MatSortModule} from '@angular/material/sort';
import {MatTableDataSource, MatTableModule} from '@angular/material/table';
import { RouterModule } from '@angular/router';
import { CommandesService } from '../../../../services/commandes.service';
import { ClientService } from '../../../../services/client.service';

@Component({
  selector: 'app-clients.component',
  imports: [MatTableModule, MatSortModule, RouterModule],
  templateUrl: './clients.component.html',
  styleUrl: './clients.component.css',
})
export class ClientsComponent {
private _liveAnnouncer = inject(LiveAnnouncer);
  displayedColumns: string[] = ['id', 'raisonSociale', 'nomClient', 'prenomClient', 'dateCreation', 'email', 'edit'];
  dataSource = new MatTableDataSource(ClientService.getAllClients());

  constructor(){

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
