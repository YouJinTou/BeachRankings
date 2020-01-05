import { Scorable } from './scorable';

export class ViewReviewModel extends Scorable {
    id: string;
    userId: string;
    beachId: string;
    text: string;
}