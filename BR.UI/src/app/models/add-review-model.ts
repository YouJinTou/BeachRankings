import { Scorable } from './scorable';

export class AddReviewModel extends Scorable {
    userId: string;
    beachId: string;
    text: string;
}