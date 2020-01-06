import { Scorable } from './scorable';
import { ViewReviewModel } from './view-review-model';

export class ViewBeachModel extends Scorable {
    id: string;
    name: string;
    addedOn: string;
    addedBy: string;
    continent: string;
    country: string;
    l1: string;
    l2: string;
    l3: string;
    l4: string;
    countryId: string;
    l1Id: string;
    l2Id: string;
    l3Id: string;
    l4Id: string;
    waterBody: string;
    location: string;
    coordinates: string;
    reviews: ViewReviewModel[];
}