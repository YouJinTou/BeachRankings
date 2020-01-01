import { Component, OnInit } from '@angular/core';
import { SearchResult } from '../../models/search.result';
import { SearchService } from '../../services/search.service';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.css']
})
export class SearchBarComponent implements OnInit {
  results: SearchResult[];

  constructor(private searchService: SearchService) { }

  ngOnInit() {
  }

  onKeyUp(query: string) {
    this.searchService.search(query).subscribe(results => {
      console.log(results);
      this.results = results;
    });
  }

  getPlaceLink(result: SearchResult) {
    let url = (result.type == 'Beach') ? environment.beachesUrl : environment.searchPlaceUrl;
    url += result.id;

    return url;
  }
}
