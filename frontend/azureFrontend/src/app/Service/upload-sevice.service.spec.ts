import { TestBed } from '@angular/core/testing';

import { UploadSeviceService } from './upload-sevice.service';

describe('UploadSeviceService', () => {
  let service: UploadSeviceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UploadSeviceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
