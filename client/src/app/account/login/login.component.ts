import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { IUser } from 'src/app/shared/models/user';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  returnUrl: string = '';
  loginForm: FormGroup = new FormGroup({
    email: new FormControl('', [
      Validators.required,
      Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$'),
    ]),
    password: new FormControl('', Validators.required),
  });
  constructor(
    private accountService: AccountService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {
    this.returnUrl =
      this.activatedRoute.snapshot.queryParams?.['returnUrl'] || '/shop';
  }

  ngOnInit(): void {}

  onSubmit() {
    console.log(this.loginForm.value);
    this.accountService.login(this.loginForm.value).subscribe({
      next: () => {
        this.router.navigateByUrl(this.returnUrl);
      },
      error: (error) => console.log(error),
    });
  }
}
