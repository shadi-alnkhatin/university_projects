import { Component, Input, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { User, UserDetails } from '../../_models/user';
import { routes } from '../../app.routes';
import { ActivatedRoute, Router, RouterLink, RouterModule } from '@angular/router';
import { AsyncPipe, CommonModule } from '@angular/common';
import { Subscription, pipe } from 'rxjs';
import { GalleryComponent, GalleryItem, } from '@daelmaak/ngx-gallery';
import { TimeagoModule } from 'ngx-timeago';
import { FriendshipService } from '../../_services/friendship.service';
import { ToastrService } from 'ngx-toastr';
import { PresenceService } from '../../_services/presence.service';
import { ChatBoxComponent } from '../chat-box/chat-box.component';

@Component({
  selector: 'app-user-details',
  standalone: true,
  imports: [RouterModule, RouterLink, CommonModule, GalleryComponent, TimeagoModule,ChatBoxComponent],
  providers: [MembersService, FriendshipService, PresenceService],
  templateUrl: './user-details.component.html',
  styleUrl: './user-details.component.css'
})
export class UserDetailsComponent implements OnInit {
  userDetails!: UserDetails
  userId = this.route.snapshot.paramMap.get('id');
  images: GalleryItem[] = [];
  onlineUsers!: string[];

  constructor(private membersServ: MembersService, private route: ActivatedRoute,
    private friendship: FriendshipService, private toast: ToastrService, public presence: PresenceService) {
  }
  ngOnInit(): void {
    this.loadDetails();
    this.presence.onlineUsers$.subscribe(res => {
      this.onlineUsers = res;
      console.log(res)
    })
  }

  loadDetails() {
    if (this.userId === null) {
      return;
    }
    this.membersServ.loadDetail(this.userId).subscribe(res => {
      this.userDetails = res;
      this.getImages();
    });
  }

  getImages() {
    if (this.userDetails && this.userDetails.photos) {
      this.images = this.userDetails.photos.map(photo => {
        const src = photo.url || '';
        //const thumb = photo.url || '';

        return {
          src
        } as GalleryItem;
      });
    } else {
      console.log('No user details or photos found');
    }
  }

  addFriend(userToSendId: string) {
    this.friendship.sendFriendRequest(userToSendId)
      .subscribe(
        {
          next: res => {
            this.toast.success("the request send it to " + this.userDetails.firstName);
            this.userDetails.isThereFriendRequest = true;
            this.userDetails.isSender = true;
          },
          error: err => {
            console.log(err)
          }

        });
  }

  rejectFriendRequest(id: number) {
    this.friendship.rejectFriendRequest(id).subscribe(
      {
        next: res => {
          this.toast.success("the request cancelled");
          this.userDetails.isThereFriendRequest = false;
          this.userDetails.isSender = false;
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
          this.userDetails.isThereFriendRequest = true;
          this.userDetails.isSender = false;
          this.userDetails.friendshipStatus = "Accepted"
        },
        error: err => {
          console.log(err)
        }

      });
  }


}
