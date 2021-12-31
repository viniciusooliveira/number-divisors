import Factor from '../models/factor';
import * as oboe from 'oboe';

const baseUrl = "http://localhost:5000";

function getFactors(number: number, callback: any){
    var config = {
      'url': `${baseUrl}/factors/${number}`,
      'method': "GET",              
      'cached': false      
    }            
    const oboeService = oboe(config);
    oboeService.node('!.*', function (data: Factor) {            
      callback({...data});
    });
}

export default{
    getFactors
}