import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';

import { SummariesService } from './summaries.service';

describe('SummariesService', () => {
  let service: SummariesService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
    });
    service = TestBed.inject(SummariesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
