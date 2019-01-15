export default class Question {
    id: number;
    name: string;
    choices: Choice[];
    answerId?: number;
}

export class Choice {
    id: number;
    name: string;
    acquire_score: number;
}