import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, delay, finalize } from "rxjs";
import { BusyService } from "../_services/busy.service";


@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
  constructor(private busy: BusyService) { }
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    this.busy.busy();
    return next.handle(request).pipe(
      delay(100),
      finalize(() => {
        this.busy.idle();
      })
    )
  }
}
