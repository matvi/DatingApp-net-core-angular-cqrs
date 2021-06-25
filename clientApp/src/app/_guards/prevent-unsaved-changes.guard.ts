import { Injectable } from '@angular/core';
import { CanActivate, CanDeactivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { EditMemberComponent } from '../members/edit-member/edit-member.component';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanActivate, CanDeactivate<unknown> {
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return true;
  }
  canDeactivate(
    component: EditMemberComponent): boolean {

      if(component.editForm.dirty){
        return confirm('Are you sure you want to leave the page without saving changes');
      }
    return true;
  }
  
}
