import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/members';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  selector: 'app-edit-member',
  templateUrl: './edit-member.component.html',
  styleUrls: ['./edit-member.component.css']
})
export class EditMemberComponent implements OnInit {
  user : User;
  member : Member;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if(this.editForm.dirty){
      $event.returnValue = true;
    }
  }

  @ViewChild('editForm') editForm : NgForm;

  constructor(private accountService: AccountService,
     private memberService : MemberService,
     private toastService : ToastrService ) { 

    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user;
      console.log('users');
      console.log(this.user);
    });
  }

  ngOnInit(): void {
    this.getMember();
  }

  getMember(){
    console.log("this goes firest" + this.user.userName);
    this.memberService.getMember(this.user.userName).subscribe(member => {
      this.member = member;
    });
  }

  updateMember()
  {
    this.memberService.updateMember(this.member).subscribe(() => {

      this.toastService.info('user updated' + this.member.nickName);
      this.editForm.reset(this.member);
      
    }, error => {
      this.toastService.error(error);
    });
  }

}
