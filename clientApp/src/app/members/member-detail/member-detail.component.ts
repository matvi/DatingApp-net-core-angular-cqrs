import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { Member } from 'src/app/_models/members';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {

   member : Member;
   galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  
  constructor(private memberService: MemberService
    , private route: ActivatedRoute ) { }

  ngOnInit(): void {
    this.getMember();
    this.galleryOptions = [
      {
        width: '600px',
        height: '400px',
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide
      },
      // max-width 800
      {
        breakpoint: 800,
        width: '100%',
        height: '600px',
        imagePercent: 80,
        thumbnailsPercent: 20,
        thumbnailsMargin: 20,
        thumbnailMargin: 20
      },
      // max-width 400
      {
        breakpoint: 400,
        preview: false
      }
    ];
    
  }

  getImages (): NgxGalleryImage[] {
    const imagesUrl : NgxGalleryImage[] = [];

    for(const photo of this.member.photos){
      imagesUrl.push({
        small : photo?.url,
        medium : photo?.url,
        big: photo?.url
      })
    }

    return imagesUrl;

  }

  getMember(){
    var username = this.route.snapshot.paramMap.get('username');
    this.memberService.getMember(username).subscribe(member =>{
      this.member = member;
      this.galleryImages = this.getImages();
    })
  }

}
