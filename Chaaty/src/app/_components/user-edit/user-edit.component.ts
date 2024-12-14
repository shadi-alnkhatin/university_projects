import { Component, OnInit, ViewChild } from '@angular/core';
import { AuthinticationService } from '../../_services/authintication-service.service';
import { User, UserDetails } from '../../_models/user';
import { IAuthResponse } from '../../_models/IAuthResponse';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, NgForm, ReactiveFormsModule, Validators } from '@angular/forms';
import { last } from 'rxjs';
import { MembersService } from '../../_services/members.service';
import { ToastrService } from 'ngx-toastr';
import { PhotoEditorComponent } from '../photo-editor/photo-editor.component';

@Component({
  selector: 'app-user-edit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule,PhotoEditorComponent],
  providers: [AuthinticationService, MembersService],
  templateUrl: './user-edit.component.html',
  styleUrl: './user-edit.component.css'
})
export class UserEditComponent implements OnInit {
  user?: UserDetails | null
  form!: FormGroup
  @ViewChild('editForm') editForm!: NgForm;
  constructor(private auth: AuthinticationService, private FormBuilder: FormBuilder, private member: MembersService,
    private toast: ToastrService) {

  }


  ngOnInit(): void {
    this.loadUserDetail();
  }
  setForm() {
    this.form = this.FormBuilder.group(
      {
        firstname: [this.user?.firstName],
        lastname: [this.user?.lastName],
        bio: [this.user?.bio],
        lookingFor: [this.user?.lookingFor],
        age: [this.user?.age],
        city: [this.user?.city],
      }
    )
  }
  submitForm(): void {
    let formData = this.form.value;
    console.log(formData);

    this.member.updateUser(formData).subscribe({
      next: (res) => {
        this.toast.success("the profile updated succesfully")
        this.editForm.reset(this.loadUserDetail());
      },
      error: (error) => {
        console.log('Update failed', error);
        this.toast.error("Update failed , please try again")
      }
    });
  }

  loadUserDetail() {
    this.member.loadDetail(localStorage.getItem('user_id')).subscribe({
      next: res => {
        this.user = res;
        console.log(res);
        this.setForm();
      },
      error: err => console.log(err)
    });
  }
}
