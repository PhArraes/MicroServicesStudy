import { Component } from '@angular/core';
import { AccessService } from '../services/access.service';
import { IAccess } from '../models/access';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  providers: [AccessService]
})
export class HomeComponent {
  public accesses: IAccess[];
  public ip: string;
  public path: string;
  constructor(private accessService: AccessService) {
    this.ip = '';
    this.path = '';
    this.change();
  }

  change() {
    this.accessService.list(this.ip, this.path, 10, 0)
      .subscribe(res => this.accesses = res, err => console.log(err));
  }
}
