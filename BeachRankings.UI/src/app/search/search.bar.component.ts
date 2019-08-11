import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { SearchResult } from '../models/search/search.result';

@Component({
    selector: 'search-bar',
    templateUrl: './search.bar.component.html',
    styleUrls: ['./search.bar.component.css']
})
export class SearchBarComponent {
    render: boolean;
    result: SearchResult;

    constructor(private httpClient: HttpClient) { }

    onSearch(event: any) {
        let query = environment.searchPlacesEndpoint + event.target.value;

        this.httpClient.get<SearchResult>(query).subscribe(result => {
            this.result = result;
            this.render = true;
        })
    }
}
