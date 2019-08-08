import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment'
import { Beach } from '../beaches/beach';

@Component({
    selector: 'search-bar',
    templateUrl: './search.bar.component.html',
    styleUrls: ['./search.bar.component.css']
})
export class SearchBarComponent {
    beaches: Beach[]

    constructor(private httpClient: HttpClient) {
    }

    onSearch(event: any) {
        let query = environment.searchPlacesEndpoint + event.target.value;

        this.httpClient.get<Beach[]>(query).subscribe(beaches => {
            this.beaches = beaches;

            console.log(this.beaches);
        })
    }
}
