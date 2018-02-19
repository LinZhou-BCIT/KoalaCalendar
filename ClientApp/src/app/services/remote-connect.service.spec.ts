import { TestBed, inject } from '@angular/core/testing';

import { RemoteConnectService } from './remote-connect.service';

describe('RemoteConnectService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RemoteConnectService]
    });
  });

  it('should be created', inject([RemoteConnectService], (service: RemoteConnectService) => {
    expect(service).toBeTruthy();
  }));
});
