import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';

import { HistorigramService } from './historigram.service';

describe('HistorigramService', () => {
  let service: HistorigramService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
    });
    service = TestBed.inject(HistorigramService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
