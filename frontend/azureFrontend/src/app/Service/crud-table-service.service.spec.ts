import { TestBed } from '@angular/core/testing';

import { CrudTableServiceService } from './crud-table-service.service';

describe('CrudTableServiceService', () => {
  let service: CrudTableServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CrudTableServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
