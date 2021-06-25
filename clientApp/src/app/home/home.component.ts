import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  registerMode = false;
  users: any;

  constructor(private http: HttpClient, private toastr : ToastrService, public accountService : AccountService) { }

  ngOnInit(): void {
    this.getUsers();
  }

  registerToggle()
  {
    this.registerMode = !this.registerMode;
  }

  getUsers()
  {
    this.http.get("https://localhost:5001/api/users").subscribe(response => {
      this.users = response;
    },
    error => {
      console.log(error);
      this.toastr.error(error.error);
    });
  }

  cancelReg(event: boolean){
    this.registerMode = event;
  }

}
