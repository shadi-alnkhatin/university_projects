import { Component, OnInit } from '@angular/core';
import { AuthinticationService } from '../../_services/authintication-service.service';
import { User } from '../../_models/user';
import { IAuthResponse } from '../../_models/IAuthResponse';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  providers: [AuthinticationService],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {


  constructor(public auth: AuthinticationService) {
    //console.log(auth.currentUser)
  }

  ngOnInit(): void {

  }
}
