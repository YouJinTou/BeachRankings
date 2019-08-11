import { Component, EventEmitter, Output } from '@angular/core';
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
    @Output() searchQuery;
    @Output() beach;

    constructor(private httpClient: HttpClient) {
        this.searchQuery = new EventEmitter<any>();
        this.beach = new EventEmitter<string>();
    }

    onSearch(event: any) {
        let query = environment.searchPlacesEndpoint + event.target.value;

        this.httpClient.get<SearchResult>(query).subscribe(result => {
            this.result = result;
            this.render = true;
        })
    }

    onLocationClick(location: string, type: string) {
        this.render = false;
        let query = {};
        query[type] = location;

        this.searchQuery.emit(query);
    }

    onBeachClick(beach: string) {
        this.render = false;

        this.beach.emit(beach);
    }
}
