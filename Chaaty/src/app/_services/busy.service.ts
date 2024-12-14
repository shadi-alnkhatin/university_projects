import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
  busyCount=0;
  constructor(private spinner : NgxSpinnerService) { }

  busy(){
    this.busyCount++;
    this.spinner.show(undefined,{
      type:"line-spin-clockwise-fade",
      bdColor:'rgba(0, 0, 0, 0.8)',
      color:'#fff'
    })
  }
  idle()
  {
    this.busyCount--;
    if(this.busyCount<=0)
      {
        this.busyCount=0;
        this.spinner.hide();
      }
  }
}
