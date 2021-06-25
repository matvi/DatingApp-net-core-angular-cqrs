import { Component, Input, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/members';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { AccountService } from 'src/app/_services/account.service';
import { User } from 'src/app/_models/user';
import { take } from 'rxjs/operators';
import { Photo } from 'src/app/_models/photo';

const baseUrl = environment.baseUrl;

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {

  constructor(private accountService: AccountService) {

    accountService.currentUser$.pipe(take(1)).subscribe((user) => {
      this.user = user;
    });

    this.uploader = new FileUploader({
      url: baseUrl + 'users/photo',
      authToken : 'Bearer ' + this.user.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    }
 
    this.hasBaseDropZoneOver = false;
 
    this.response = '';
 
    this.uploader.response.subscribe( res => this.response = res );

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const photo: Photo = JSON.parse(response);
        this.member.photos.push(photo);
         if (photo.isMain) {
           this.user.photoUrl = photo.url;
           this.member.photoUrl = photo.url;
           this.accountService.setCurrentUser(this.user);
         }
      }
    }
   }
   
  @Input() member : Member;
  uploader:FileUploader;
  hasBaseDropZoneOver:boolean;
  response:string;
  user : User;

  ngOnInit(): void {
  }

  public fileOverBase(e:any):void {
    this.hasBaseDropZoneOver = e;
  }
}
