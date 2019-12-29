import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Beach } from 'src/app/models/search/beach';
import { environment } from '../../environments/environment';

@Component({
  selector: 'beach-details',
  templateUrl: './beach.component.html',
  styleUrls: ['./beach.component.css']
})
export class BeachComponent implements OnInit {
  beach: Beach;

  constructor(private route: ActivatedRoute, private httpClient: HttpClient) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.httpClient.get<Beach>(environment.beachesEndpoint + params['id'])
        .subscribe(beach => this.beach = beach);
    });
  }
}