import { Component, Inject, OnInit } from '@angular/core';
import { User } from '../../types/users';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css'],
})
export class UsersComponent implements OnInit {
  users: User[] = [];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<User[]>(`${baseUrl}api/users`).subscribe(
      (result) => {
        this.users = result;
      },
      (error) => console.error(error)
    );
  }

  ngOnInit(): void {}
}
