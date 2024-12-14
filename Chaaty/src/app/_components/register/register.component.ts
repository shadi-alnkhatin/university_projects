import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthinticationService } from '../../_services/authintication-service.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Countries, countries } from '../../_models/Countries';


@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule,],
  providers: [AuthinticationService],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  countries: Countries[] = countries;
  form!: any;
  constructor(private formBuilder: FormBuilder,
    private auht: AuthinticationService, private toast: ToastrService, private router: Router) {
    this.SetFormBuilder();
  }


  SetFormBuilder() {
    this.form = this.formBuilder.group(
      {
        email: ["", [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(8)]],
        firstName: ['', [Validators.required]],
        lastName: ['', [Validators.required]],
        country: ['', [Validators.required]],
        age: ['', [Validators.required, Validators.min(18)]],
        gender: ['', [Validators.required]]

      }
    )
  };

  onSubmit() {
    if (this.form.valid) {
      const email = this.form.get('email')?.value;
      const password = this.form.get('password')?.value;
      const firstName = this.form.get('firstName')?.value;
      const lastName = this.form.get('lastName')?.value;
      const age = this.form.get('age')?.value;
      const country = this.form.get('country')?.value;
      const gender = this.form.get('gender')?.value;


      this.auht.register({
        email: email, password: password, firstName: firstName, lastName: lastName, gender: gender,
        age: age, country: country
      }).subscribe({
        next: response => {
          this.toast.success("user registered and login success");
          try {
            this.router.navigateByUrl('/users')
          } catch (e) {
            console.log('Non-JSON response:', response);
          }
        },
        error: error => {
          if (error.status === 200) {
            console.log('200 status but:', error.error.text);
          } else {
            console.log(error.status + error);
          }
        },
      });
    }
  }



}
