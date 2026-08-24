import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-landing',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss']
})
export class LandingComponent implements OnInit {
  mostrarLoginModal = false;
  cargando = false;
  mensajeError: string | null = null;
  loginForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      nombreUsuario: ['', [Validators.required, Validators.pattern('^[0-9]{8,9}$')]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  abrirModalLogin(): void {
    this.mensajeError = null;
    this.mostrarLoginModal = true;
  }

  cerrarModalLogin(): void {
    this.mostrarLoginModal = false;
    this.loginForm.reset();
  }

  onLoginSubmit(): void {
    if (this.loginForm.invalid) return;

    this.cargando = true;
    this.mensajeError = null;

    this.authService.login(this.loginForm.value).subscribe({
      next: () => {
        this.cargando = false;
        this.cerrarModalLogin();
        // Redirige al panel de administración tras ingresar
        this.router.navigate(['/admin']);
      },
      error: (err) => {
        this.cargando = false;
        this.mensajeError = err.error?.mensaje || 'Credenciales incorrectas o cuenta bloqueada.';
      }
    });
  }
}