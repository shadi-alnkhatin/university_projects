import { ChangeDetectorRef, Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MembersService } from '../../_services/members.service';
import { ToastrService } from 'ngx-toastr';
import { User } from '../../_models/user';
import { CommonModule } from '@angular/common';
import { tap } from 'rxjs';


@Component({
  selector: 'app-photo-editor',
  standalone: true,
  imports: [FormsModule, CommonModule, ReactiveFormsModule],
  providers: [MembersService],
  templateUrl: './photo-editor.component.html',
  styleUrl: './photo-editor.component.css'
})
export class PhotoEditorComponent {
  @Input() user!: User;
  @Output() reloadParent = new EventEmitter<void>();
  selectedFile: File | null = null;
  uploadForm!: FormGroup;


  constructor(private member: MembersService, private toast: ToastrService,
    private fb: FormBuilder, private cdr: ChangeDetectorRef) {
    this.uploadForm = this.fb.group({
      fileInput: ['']
    });
  }


  onFileSelected(event: Event) {
    const fileInput = event.target as HTMLInputElement;
    if (fileInput.files && fileInput.files.length > 0) {
      this.selectedFile = fileInput.files[0];
    }
  }

  onSubmit() {
    if (this.selectedFile) {
      this.member.uploadPhoto(this.selectedFile).subscribe({
        next: response => {
          console.log(response);
          this.toast.success("Image uploaded successfully");
          this.addPhotoToUser(response);
          this.resetForm();
        },
        error: error => {
          console.error(error);
          this.toast.error("Image upload failed");
        }
      });
    }
  }

  resetForm() {
    this.uploadForm.reset();
    this.selectedFile = null;
    const fileInputElement = document.getElementById('formFile') as HTMLInputElement;
    if (fileInputElement) {
      fileInputElement.value = '';
    }
  }

  deletePhoto(id: number) {
    this.member.deletePhoto(id).subscribe({
      next: res => {
        this.toast.info("photo deleted successfully")
        this.user.photos = this.user.photos.filter(photo => photo.id !== id);
      }
    })
  }

  addPhotoToUser(photo: any) {
    this.user.photos.push(photo);
    this.cdr.detectChanges();
  }

  setMain(id: number) {

    this.member.updateMainPhoto(id).pipe(
      tap(() => {
        this.toast.show("Main photo is changed!");
        this.updatePhotoIsMain(id);
        this.reloadParent.emit();
      })
    ).subscribe({
      error: err => {
        this.toast.error("Failed to set main photo.");
        console.error(err);
      }
    });
  }

  private updatePhotoIsMain(id: number) {
    this.user.photos.forEach(photo => {
      photo.isMain = photo.id === id;
    });
  }
}
