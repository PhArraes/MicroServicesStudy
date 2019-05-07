import { AccessService } from './../services/access.service';
import { Component } from '@angular/core';
import { IAccess } from '../models/access';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html',
  providers: [AccessService]
})
export class FetchDataComponent {
  public accesses: IAccess[];

  constructor(private accessService: AccessService) {
    accessService.list(50, 10).subscribe(res => this.accesses = res, err => console.log(err));
  }
}
