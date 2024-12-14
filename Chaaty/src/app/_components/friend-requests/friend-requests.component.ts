import { Component, OnInit } from '@angular/core';
import { FriendshipService } from '../../_services/friendship.service';
import { Friendship } from '../../_models/friendship';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-friend-requests',
  standalone: true,
  imports: [CommonModule],
  providers: [FriendshipService],
  templateUrl: './friend-requests.component.html',
  styleUrl: './friend-requests.component.css'
})
export class FriendRequestsComponent implements OnInit {
  friendsRequests: Friendship[] = [];
  constructor(private friendship: FriendshipService, private toast: ToastrService) {

  }
  ngOnInit(): void {
    this.loadFriendsrequests();
  }

  loadFriendsrequests() {
    this.friendship.getFriendsRequest().subscribe(res => {
      this.friendsRequests = res;
      console.log(res);
    });
  }
  rejectFriendRequest(id: number) {
    this.friendship.rejectFriendRequest(id).subscribe(
      {
        next: res => {
          this.toast.success("the request cancelled");
          this.loadFriendsrequests();

        },
        error: err => {
          console.log(err)
        }

      });
  }

  approveFriendRequest(id: number) {
    this.friendship.acceptFriendRequest(id).subscribe(
      {
        next: res => {
          this.toast.success("the request approved");
          this.loadFriendsrequests();
        },
        error: err => {
          console.log(err)
        }

      });
  }
}
