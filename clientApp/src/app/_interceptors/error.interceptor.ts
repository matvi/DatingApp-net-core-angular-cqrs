import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NavigationCancel, NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toastr : ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(error => {
        if(error){
          console.log(error);
          switch(error.status){
            case 400:
              var errores = error.error.errors;
              if(errores){
                const modalStateErrors = [];
                for(const key in errores){
                  modalStateErrors.push(errores[key]);
                }
                throw modalStateErrors;
              }
              this.toastr.error(error.statusText, error.status);
              this.router.navigateByUrl('/not-found');
              break;
            case 401:
              this.toastr.error(error.statusText, error.status);
              this.router.navigateByUrl('/not-found');
              break;
            case 404:
              this.toastr.error(error.statusText, error.status);
              this.router.navigateByUrl('/not-found');
              break;
            case 500:
              this.toastr.error(error.statusText, error.status);
              const navitationExtras : NavigationExtras = { state: { error: error.error}}
              console.log("printinr error");
              console.log(error.error);
              this.router.navigateByUrl('/server-error', navitationExtras);
              break;
            default:
              this.toastr.error('Something went wrong. Interception the error');
              break;
          }
        }
        return throwError(error);
      })
    )
  }
}
