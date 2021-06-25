import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/members';



@Injectable({
  providedIn: 'root'
})

export class MemberService {
  private baseUrl = environment.baseUrl;
  members : Member[] = [];

  // private httpOptions = {
  //   headers : new HttpHeaders({
  //     Authorization : 
  //       'Bearer ' + JSON.parse(localStorage.getItem('user')).token
  //   })
  // }

  constructor(private http : HttpClient) { }

  getMembers(){
    return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
      map(members =>{
        this.members = members;
        return members;
      })
    );
  }

  getMember(username : string){
    var member = this.members.find(member => member.userName === username);
    if(member !== undefined)
    {
      return of(member);
    }

    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  updateMember(member : Member){

    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(()=> {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    );
  }
}



