import { TestBed } from '@angular/core/testing';

import { MessageRelayServiceService } from './message-relay-service.service';

describe('MessageRelayServiceService', () => {
  let service: MessageRelayServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MessageRelayServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
