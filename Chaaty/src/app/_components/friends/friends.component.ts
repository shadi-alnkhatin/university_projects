import { Component, OnInit } from '@angular/core';
import { FriendshipService } from '../../_services/friendship.service';
import { Friendship } from '../../_models/friendship';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-friends',
  standalone: true,
  imports: [CommonModule],
providers:[FriendshipService],
  templateUrl: './friends.component.html',
  styleUrl: './friends.component.css'
})
export class FriendsComponent implements OnInit {
  Friends:Friendship[]=[];
  constructor(private friendService:FriendshipService){}
  ngOnInit(): void {
    this.loadFriends();
  }


 loadFriends()
 {
    this.friendService.getFriends().subscribe(res=>{
      this.Friends=res;
    })
 }

}
