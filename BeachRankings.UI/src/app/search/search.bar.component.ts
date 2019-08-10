import { Component, EventEmitter, Output } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment'
import { SearchResult } from './models/search.result';

@Component({
    selector: 'search-bar',
    templateUrl: './search.bar.component.html',
    styleUrls: ['./search.bar.component.css']
})
export class SearchBarComponent {
    render: boolean;
    result: SearchResult;
    @Output() searchQuery;

    constructor(private httpClient: HttpClient) {
        this.searchQuery = new EventEmitter<any>();
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

        console.log(query);

        this.searchQuery.emit(query);
    }

    onBeachClick(beach: string) {
        this.render = false;
        
        console.log('NOT IMPLEMENTED');
    }
}
