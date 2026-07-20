import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-service.component',
  imports: [],
  templateUrl: './service.component.html',
  styleUrl: './service.component.css',
})
export class ServiceComponent implements OnInit{
  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }

}
