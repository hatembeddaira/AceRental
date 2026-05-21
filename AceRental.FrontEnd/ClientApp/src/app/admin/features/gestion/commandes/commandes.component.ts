import {LiveAnnouncer} from '@angular/cdk/a11y';
import {AfterViewInit, Component, ViewChild, inject} from '@angular/core';
import {MatSort, Sort, MatSortModule} from '@angular/material/sort';
import {MatTableDataSource, MatTableModule} from '@angular/material/table';
import { RouterModule } from '@angular/router';
import { CommandesService } from '../../../../services/commandes.service';

@Component({
  selector: 'app-commandes.component',
  imports: [MatTableModule, MatSortModule, RouterModule],
  templateUrl: './commandes.component.html',
  styleUrl: './commandes.component.css',
})
export class CommandesComponent {
  private _liveAnnouncer = inject(LiveAnnouncer);
  displayedColumns: string[] = ['id', 'nomClient', 'prenomClient', 'raisonSociale', 'dateCreation', 'dateDebutReservation', 'dateFinReservation', 'derniereModification', 'prix', 'statut', 'edit'];
  dataSource = new MatTableDataSource(CommandesService.getAllCommandes());

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
