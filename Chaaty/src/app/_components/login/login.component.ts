import { CommonModule } from '@angular/common';
import { Component, OnInit, } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthinticationService } from '../../_services/authintication-service.service';
import { ToastrService, provideToastr } from 'ngx-toastr';
import { User } from '../../_models/user';
import { routes } from '../../app.routes';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule,],
  providers: [AuthinticationService],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  form!: any;
  constructor(private formBuilder: FormBuilder,
    private auht: AuthinticationService, private toast: ToastrService,private router: Router) {
    this.SetFormBuilder();
  }


  SetFormBuilder() {
    this.form = this.formBuilder.group(
      {
        email: ["", [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(8)]]
      }
    )
  };

  onSubmit() {
    if (this.form.valid) {
      const email = this.form.get('email')?.value;
      const password = this.form.get('password')?.value;
      this.auht.login({ email: email, password: password }).subscribe({
        next: response => {
          this.toast.success("user login success");
          // Attempt to handle non-JSON responses
          try {

            console.log(response + "User Login");
            this.router.navigateByUrl('/users')
          } catch (e) {
            console.log('Non-JSON response:', response);
          }
        },
        error: error => {
          this.toast.error("you make a mistake");
          if (error.status === 200) {
            console.log('200 status but:', error.error.text);
          } else {
            console.log(error.status + error);
          }
        },
      });
    } else {
      console.log('Form is invalid');
    }

  }


  ngOnInit() {
  }

}
