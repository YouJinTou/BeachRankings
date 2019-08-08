import * as tslib_1 from "tslib";
import { Component } from '@angular/core';
import { Subject } from 'rxjs';
let StatsTableComponent = class StatsTableComponent {
    constructor(httpClient) {
        this.httpClient = httpClient;
        this.dtOptions = {};
        this.dtTrigger = new Subject();
    }
    ngOnInit() {
        this.dtOptions = {
            pagingType: 'full_numbers',
            pageLength: 2
        };
        this.httpClient.get('https://localhost:44375/api/search/beaches?prefix=b')
            .subscribe(b => {
            this.dtTrigger.next();
        });
    }
    ngOnDestroy() {
        this.dtTrigger.unsubscribe();
    }
    extractData(res) {
        const body = res.json();
        return body || {};
    }
};
StatsTableComponent = tslib_1.__decorate([
    Component({
        templateUrl: 'stats.table.component.html',
        selector: 'stats-table'
    })
], StatsTableComponent);
export { StatsTableComponent };
//# sourceMappingURL=stats.table.component.js.map