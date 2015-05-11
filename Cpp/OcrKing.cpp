
#include <curlpp/cURLpp.hpp>
#include <curlpp/Easy.hpp>
#include <curlpp/Options.hpp>
#include <curlpp/Exception.hpp>


int main(int argc, char *argv[]){

	try {
		curlpp::Cleanup cleaner;
		curlpp::Easy request;

		request.setOpt(new curlpp::options::Url("http://lab.ocrking.com/ok.html"));
		//request.setOpt(new curlpp::options::Verbose(true)); 

		{
			curlpp::Forms formParts;
			formParts.push_back(new curlpp::FormParts::File("file", "XXXX.jpg"));
			formParts.push_back(new curlpp::FormParts::Content("service", "OcrKingForNumber"));
			formParts.push_back(new curlpp::FormParts::Content("language", "eng"));
			formParts.push_back(new curlpp::FormParts::Content("type", "http://www.XXXXXXX.com/validateCode"));
			formParts.push_back(new curlpp::FormParts::Content("charset", "7"));
			formParts.push_back(new curlpp::FormParts::Content("apiKey", "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"));

			request.setOpt(new curlpp::options::HttpPost(formParts)); 
		}

		request.perform();
		std::cout << request;
	} catch ( curlpp::LogicError & e ) {
		std::cout << e.what() << std::endl;
	} catch ( curlpp::RuntimeError & e ) {
		std::cout << e.what() << std::endl;
	}
	std::cout << std::endl;

	return EXIT_SUCCESS;
}
