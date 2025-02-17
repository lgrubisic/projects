import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgForm, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { TimeTrackService } from '../../services/time-track.service';
import { formatDate } from '@angular/common';
import { EmployeeInfoService } from '../../services/employee-info.service';


@Component({ 
  selector: 'app-time-tracking',
  templateUrl: './new-time-tracking.component.html',
  styleUrls: ['./new-time-tracking.component.css'],
  encapsulation: ViewEncapsulation.None,
})

export class NewTimeTrackingComponent implements OnInit {
  today = new Date();
  date  =  new  FormControl(new  Date());


  constructor(private timeService: TimeTrackService, private toastr: ToastrService, public service: EmployeeInfoService) { }

  ngOnInit(): void {
    this.resetForm();
  }
  /**
   * Add time record to server
   * @param timeForm form that contains time records
   */
  onSubmit(timeForm: NgForm) {
    if (this.timeService.timeFormData.timer_id == 0) {
      this.insertRecord(timeForm);
    }
    else {
      this.updateRecord(timeForm);
    }
    this.resetForm();
  }
  /**
   * Updates record with new time form data
   * @param timeForm orm that contains time records
   */
  updateRecord(timeForm: NgForm) {
    this.timeService.putTimeTracking().subscribe(
      res => {
        this.resetForm(timeForm);
        this.toastr.info('Updated successfully', 'Time Tracking Register');
        this.timeService.refreshTimeList();
      },
      err => {
        this.toastr.error(err.message, "Error!")
      }
    )
  }
  /**
   * Insert new time record to server.
   * @param timeForm time form data 
   */
  insertRecord(timeForm: NgForm) {
    this.timeService.postTimeTracking().subscribe(
      res => {
        this.resetForm(timeForm);
        this.timeService.refreshTimeList();
      },
      err => { 
        this.toastr.error(err.message, "Error!")
      }
    )
  }
  /**
   * Turns timer input forms to default values.
   * @param timeForm time form 
   */
  resetForm(timeForm?: NgForm) {
    if (timeForm != null)
    timeForm.form.reset();
    this.timeService.timeFormData = {
      timer_id: 0,
      employee_init_id: 0,
      date_of_work: formatDate(new Date(), 'yyyy-MM-dd', 'en'),
      time_in: formatDate(this.today, 'HH:mm:ss', 'en-US'),
      time_out: formatDate(this.today, 'HH:mm:ss', 'en-US', '+1000'),
        }
  }  

}
