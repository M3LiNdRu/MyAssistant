export interface MonthlySummary {
    month: string;
    year: string;
    start: number;
    saved: number;
    progressBar: Dictionary<number>;
    spentByCategory: Dictionary<number>
}

interface Dictionary<T> {
    [Key: string]: T;
}