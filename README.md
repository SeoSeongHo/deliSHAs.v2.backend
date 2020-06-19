# deliSHAs.v2.backend

서울대학교 학식 어플리케이션 백엔드 프로젝트로, 크롤러, 캐시 등을 이용한 효율적인 서버를 구축하였습니다.

## Features
- C# .net core
- Inmemory Cache
- AWS Beanstalk Using Docker & Dockerrun.aws.json

***

## Focus

### Low Latency (지연시간)
비동기 처리를 통하여 Response 를 빠르게 반환함으로써, Low Latency 를 보장하는 WAS 를 구현

### Efficient (효율성)
새벽 시간대에 배치로 돌아가는 크롤러를 통해, DB 에 데이터를 쌓고, 캐시를 이용하여, DB 트랜잭션을 최소화
  
***

## Architecture

### WAS
WAS 는 클라이언트의 요청을 받아, 캐시에 해당 날짜 데이터가 있다면 바로 반환, 없다면 DB 를 통해 데이터를 가져오고, 캐시에 저장 후, 반환하는 역할을 담당하고 있습니다.

***

### Crawler
[deliSHAs crawler](https://github.com/BaekGeunYoung/deliSHAs_crawler)

***
