import { AfterViewInit, Component, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { User } from '../../_models/user';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Observable } from 'rxjs';
import { Pagination } from '../../_models/Pagination';
import { PaginationComponent } from '../pagination/pagination.component';
import { PresenceService } from '../../_services/presence.service';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, RouterModule, PaginationComponent],
  providers: [MembersService, ],

  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.css'
})
export class UserListComponent implements OnInit, AfterViewInit {
  users!: User[];
  pagination!: Pagination;
  page = 1;
  pageSize = 4;
  onlineUsers:string[]=[]

  constructor(private membersServ: MembersService, public presence: PresenceService) {

  }
  ngAfterViewInit(): void {

  }
  ngOnInit(): void {
    this.getUsers();

    setTimeout(() => {
     this.presence.onlineUsers$.subscribe({
        next: (users: string[]) => {
          this.onlineUsers = users;
          console.log('Updated online users list:', this.onlineUsers);
          console.log('Updated online users list:', users);

        },
        error: (error) => console.error('Error receiving online users: ' + error)
      });

    }, 1000);

  }

  getUsers() {
    this.membersServ.loadMembers(this.page, this.pageSize).subscribe(res => {
      this.users = res.result;
      this.pagination = res.pagination;
    })
  }

  onPageChange(page: number): void {
    this.page = page;
    this.getUsers();
  }
}
